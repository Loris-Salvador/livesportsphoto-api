using Application.Repositories;
using AutoMapper;
using Domain.Models;
using Google.Cloud.Firestore;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public class FireStoreSectionRepository : ISectionRepository
{
    private const string CollectionName = "Sections";

    public FireStoreSectionRepository(FirestoreDb firestoreDb, IMapper mapper)
    {
        FirestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private FirestoreDb FirestoreDb { get; }

    private IMapper Mapper { get; }

    public async Task<Section> AddAsync(Section section, CancellationToken cancellationToken)
    {
        var collection = FirestoreDb.Collection(CollectionName);
        var sectionDocument = new SectionDocument()
        {
            Name = section.Name,
        };

        var documentRef = await collection.AddAsync(sectionDocument, cancellationToken);

        var snapshot = await documentRef.GetSnapshotAsync(cancellationToken);

        return Mapper.Map<Section>(snapshot.ConvertTo<SectionDocument>());
    }

    public async Task<Album> AddAlbumAsync(string sectionId, Album album, CancellationToken cancellationToken)
    {

        var albums = FirestoreDb.Collection(CollectionName).Document(sectionId).Collection("Albums");

        var albumDocument = new AlbumDocument
        {
            Name = album.Name,
            Link = album.Link
        };

        var documentRef =  await albums.AddAsync(albumDocument, cancellationToken);

        var snapshot = await documentRef.GetSnapshotAsync(cancellationToken);

        return Mapper.Map<Album>(snapshot.ConvertTo<AlbumDocument>());
    }

    public async Task<List<Section>> ToList(CancellationToken cancellationToken)
    {
        var sectionCollection = FirestoreDb.Collection(CollectionName);

        var snapshot = await sectionCollection.GetSnapshotAsync(cancellationToken);

        var sections = new List<Section>();

        foreach (var sectionRef in snapshot.Documents)
        {
            var sectionDocument = sectionRef.ConvertTo<SectionDocument>();

            var sectionModel = Mapper.Map<Section>(sectionDocument);

            var albumsCollection =  sectionCollection.Document(sectionDocument.Id).Collection("Albums");
            var albumsSnapshot = await albumsCollection.GetSnapshotAsync(cancellationToken);

            foreach (var album in albumsSnapshot.Documents)
            {
                var albumDocument = album.ConvertTo<AlbumDocument>();

                sectionModel.Albums.Add(Mapper.Map<Album>(albumDocument));
            }

            sections.Add(sectionModel);
        }

        return sections;
    }
}