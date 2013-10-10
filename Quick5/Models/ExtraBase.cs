namespace Quick5.Models
{
    public class ExtraBase : SqlBase
    {
        public ExtraBase() : base("Extra") { }

        public Sirens Sirens
        {
            get
            {
                if (_Sirens == null) _Sirens = new Sirens(this.connexion);
                return _Sirens;
            }
            set
            {
                _Sirens = value;
            }

        }
        private Sirens _Sirens;

        public Clients Clients
        {
            get
            {
                if (_Clients == null) _Clients = new Clients(this.connexion);
                return _Clients;
            }
            set
            {
                _Clients = value;
            }

        }
        private Clients _Clients;

        public Garanties Garanties
        {
            get
            {
                if (_Garanties == null) _Garanties = new Garanties(this.connexion);
                return _Garanties;
            }
            set
            {
                _Garanties = value;
            }

        }
        private Garanties _Garanties;

        public Decisions Decisions
        {
            get
            {
                if (_Decisions == null) _Decisions = new Decisions(this.connexion);
                return _Decisions;
            }
            set
            {
                _Decisions = value;
            }

        }
        private Decisions _Decisions;

        public EdiAccords EdiAccords
        {
            get
            {
                if (_EdiAccords == null) _EdiAccords = new EdiAccords(this.connexion);
                return _EdiAccords;
            }
            set
            {
                _EdiAccords = value;
            }

        }
        private EdiAccords _EdiAccords;
    }
}
