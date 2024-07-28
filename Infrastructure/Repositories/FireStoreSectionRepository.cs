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

        var document =  snapshot.ConvertTo<SectionDocument>();

        return Mapper.Map<Section>(document);
    }
}