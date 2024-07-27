using Google.Cloud.Firestore;

namespace Infrastructure.Models;

[FirestoreData]
public class AlbumDocument
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public required string Name { get; set; }

    [FirestoreProperty]
    public required string Link { get; set; }
}