namespace CloudQA.Task1.Models;

public class ElementCandidate
{
    public string TagName { get; set; } = "";
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string Placeholder { get; set; } = "";
    public string Text { get; set; } = "";
    public string NearbyText { get; set; } = "";
    public int Score { get; set; }

    public override string ToString()
    {
        return $"Tag={TagName}, Id={Id}, Name={Name}, Type={Type}, " +
               $"Placeholder={Placeholder}, NearbyText={NearbyText}, Score={Score}";
    }
}