using Utils.DB;
public class InfrastructureConnectionString: ConnectionString, IInfrastructureConnectionString { 


    public InfrastructureConnectionString(ConnectionString cs)  {

        this.Database = cs.Database;
        this.Host = cs.Host;
        this.Name = cs.Name;
        this.Password = cs.Password;
        this.Port = cs.Port;
        this.User = cs.User;
    }

    public InfrastructureConnectionString()
    {
        
    }
}
public interface IInfrastructureConnectionString: IConnection 
{

}