namespace Quick5.Models
{
    public class ExtraBase : SqlBase
    {
        public ExtraBase() : base("Extra.Dev") { }

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

        public Agences Agences
        {
            get
            {
                if (_Agences == null) _Agences = new Agences(this.connexion);
                return _Agences;
            }
            set
            {
                _Agences = value;
            }
        }
        private Agences _Agences;

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

        public PxGroupes PxGroupes
        {
            get
            {
                if (_PxGroupes == null) _PxGroupes = new PxGroupes(this.connexion);
                return _PxGroupes;
            }
            set
            {
                _PxGroupes = value;
            }
        }
        private PxGroupes _PxGroupes;

        public PxSites PxSites
        {
            get
            {
                if (_PxSites == null) _PxSites = new PxSites(this.connexion);
                return _PxSites;
            }
            set
            {
                _PxSites = value;
            }
        }
        private PxSites _PxSites;
    }
}
