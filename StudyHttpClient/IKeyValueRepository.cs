namespace StudyHttpClient
{
    public interface IKeyValueRepository
    {
        KeyValue Find(string key);
        void Create(KeyValue keyValue);
        void Update(string key, string updatedValue);
    }
}