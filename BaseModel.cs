namespace Mutify
{
    public class BaseModel: PrimaryKeyModel
    {
    }

    public abstract class PrimaryKeyModel
    {
        public int Id { get; set; }
    }
}