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
}