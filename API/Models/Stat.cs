namespace API.Models
{
    public class Stat<T>
    {
        public string Label { get; set; }
        public T Value { get; set; }

        public Stat(string label, T value)
        {
            this.Label = label;
            this.Value = value;
        }
    }
}
