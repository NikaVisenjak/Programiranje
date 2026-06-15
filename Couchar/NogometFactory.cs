namespace Couchar
{
    public class NogometFactory : SportFactory
    {
        public override Sport UstvariSport() => new Nogomet();
    }
}
