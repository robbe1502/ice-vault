namespace IceVault.Common.Settings;

public class PersistenceSetting
{
    public Write Write { get; set; }

    public Read Read { get; set; }
}

public class Write
{
    public string ConnectionString { get; set; }

    public bool IsLogsEnabled { get; set; }
}

public class Read
{

}