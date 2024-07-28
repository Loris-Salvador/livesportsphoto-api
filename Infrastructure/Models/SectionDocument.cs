using Google.Cloud.Firestore;

namespace Infrastructure.Models;

[FirestoreData]
public class SectionDocument
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public required string Name { get; set; }
}