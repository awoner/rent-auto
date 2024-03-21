using System.ComponentModel.DataAnnotations;

namespace RentAutoPoc.Api.Settings;

public class AppSettings
{
    [Required]
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    [Required]
    public MongoConnectionStrings MongoConnectionStrings { get; set; }
    
    [Required]
    public ElasticsearchConnectionStrings ElasticsearchConnectionStrings { get; set; }
}

public class MongoConnectionStrings
{
    [Required(AllowEmptyStrings = false)]
    public string ConnectionString { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string DatabaseName { get; set; }
}

public class ElasticsearchConnectionStrings
{
    [Required(AllowEmptyStrings = false)]
    public string Url { get; set; }
}