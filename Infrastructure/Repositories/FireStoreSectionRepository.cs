using Application.Repositories;
using AutoMapper;
using Domain.Models;
using Google.Cloud.Firestore;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public class FireStoreSectionRepository : ISectionRepository
{
    private const string SectionCollectionName = "Sections";

    private const string AlbumsCollectionName = "Albums";


    public FireStoreSectionRepository(FirestoreDb firestoreDb, IMapper mapper)
    {
        FirestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private FirestoreDb FirestoreDb { get; }

    private IMapper Mapper { get; }

    public async Task<Section> AddAsync(Section section, CancellationToken cancellationToken)
    {
        var collection = FirestoreDb.Collection(SectionCollectionName);
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

        var albums = FirestoreDb.Collection(SectionCollectionName).Document(sectionId).Collection(AlbumsCollectionName);

        var albumDocument = new AlbumDocument
        {
            Name = album.Name,
            Link = album.Link
        };

        var documentRef =  await albums.AddAsync(albumDocument, cancellationToken);

        var snapshot = await documentRef.GetSnapshotAsync(cancellationToken);

        return Mapper.Map<Album>(snapshot.ConvertTo<AlbumDocument>());
    }

    public async Task<List<Section>> ToListAsync(CancellationToken cancellationToken)
    {
        var sectionCollection = FirestoreDb.Collection(SectionCollectionName);

        var snapshot = await sectionCollection.GetSnapshotAsync(cancellationToken);

        var sections = new List<Section>();

        foreach (var sectionRef in snapshot.Documents)
        {
            var sectionDocument = sectionRef.ConvertTo<SectionDocument>();

            var sectionModel = Mapper.Map<Section>(sectionDocument);

            var albumsCollection =  sectionCollection.Document(sectionDocument.Id).Collection(AlbumsCollectionName);
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

    public async Task<Album> DeleteAlbumAsync(string sectionId, string albumId, CancellationToken cancellationToken = default)
    {
        var albumReference = FirestoreDb.Collection(SectionCollectionName)
            .Document(sectionId)
            .Collection(AlbumsCollectionName)
            .Document(albumId);

        var albumDocument = await albumReference.GetSnapshotAsync(cancellationToken);

        await albumReference.DeleteAsync(cancellationToken: cancellationToken);

        return Mapper.Map<Album>(albumDocument.ConvertTo<AlbumDocument>());
    }

    public async Task<List<Album>> GetAlbumAsync(string sectionId, CancellationToken cancellationToken = default)
    {
        var albumCollection = FirestoreDb.Collection(SectionCollectionName).Document(sectionId).Collection(AlbumsCollectionName);

        var snapshot = await albumCollection.GetSnapshotAsync(cancellationToken);

        var albums = new List<Album>();

        foreach (var album in snapshot.Documents)
        {
            albums.Add(Mapper.Map<Album>(album.ConvertTo<AlbumDocument>()));
        }

        return albums;
    }

    public async Task<Section> DeleteAsync(string sectionId, CancellationToken cancellationToken = default)
    {
        var sectionReference = FirestoreDb.Collection(SectionCollectionName)
            .Document(sectionId);

        var sectionDocument = await sectionReference.GetSnapshotAsync(cancellationToken);

        await sectionReference.DeleteAsync(cancellationToken: cancellationToken);

        return Mapper.Map<Section>(sectionDocument.ConvertTo<SectionDocument>());
    }
}