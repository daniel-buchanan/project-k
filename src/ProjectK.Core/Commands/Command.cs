namespace ProjectK.Core.Commands
{
    public class Command : ICommand
    {
        private string _json;

        public virtual void Initialise(string json)
        {
            _json = json;
        }

        public string Json => _json;
    }
}
