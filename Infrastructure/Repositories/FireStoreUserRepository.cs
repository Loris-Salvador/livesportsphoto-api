using Application.Repositories;
using AutoMapper;
using Domain.Models;
using Google.Cloud.Firestore;
using Infrastructure.Models;
using static System.Collections.Specialized.BitVector32;
using System.Threading;
namespace Infrastructure.Repositories;

public class FireStoreUserRepository : IUserRepository
{
    public FireStoreUserRepository(FirestoreDb firestoreDb, IMapper mapper)
    {
        FirestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private FirestoreDb FirestoreDb { get; }

    private IMapper Mapper { get; }

    public async Task<User> GetUser(string name, CancellationToken cancellationToken)
    {
        var user = FirestoreDb.Collection("Users").Document(name);

        var snapshot = await user.GetSnapshotAsync(cancellationToken);

        if (!snapshot.Exists)
        {
            return null;
        }

        return Mapper.Map<User>(snapshot.ConvertTo<UserDocument>());
    }
}