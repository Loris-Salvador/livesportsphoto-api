using Google.Cloud.Firestore;

namespace Infrastructure.Models;

[FirestoreData]
public class UserDocument
{
    [FirestoreDocumentId]
    public required string UserName { get; set; }

    [FirestoreProperty]
    public required string Password { get; set; }
}