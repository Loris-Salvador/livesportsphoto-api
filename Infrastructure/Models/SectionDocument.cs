using Google.Cloud.Firestore;

namespace Infrastructure.Models;

[FirestoreData]
public class SectionDocument
{
    [FirestoreDocumentId]
    public required string Name { get; set; }
}