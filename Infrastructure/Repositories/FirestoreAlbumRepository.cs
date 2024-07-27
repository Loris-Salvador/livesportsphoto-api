using Application.Repositories;
using Domain.Models;
using Google.Cloud.Firestore;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public class FirestoreAlbumRepository : IAlbumRepository
{

    private const string CollectionName = "Albums";

    public FirestoreAlbumRepository(FirestoreDb firestoreDb)
    {
        FirestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
    }

    private FirestoreDb FirestoreDb { get; }

    public async Task<Album> AddAlbumAsync(Album album, CancellationToken cancellationToken)
    {
        var collection = FirestoreDb.Collection(CollectionName);
        var albumDocument = new AlbumDocument
        {
            Name = album.Name,
            Link = album.Link
        };

        var documentRef = await collection.AddAsync(albumDocument, cancellationToken);

        var snapshot = await documentRef.GetSnapshotAsync(cancellationToken);

        var document =  snapshot.ConvertTo<AlbumDocument>();

        return new Album
        {
            Id = document.Id,
            Name = document.Name,
            Link = document.Link
        };

    }
}