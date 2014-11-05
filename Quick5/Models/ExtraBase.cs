﻿namespace Quick5.Models
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

        public Communes Communes
        {
            get
            {
                if (_Communes == null) _Communes = new Communes(this.connexion);
                return _Communes;
            }
            set
            {
                _Communes = value;
            }
        }
        private Communes _Communes;

        public Insees Insees
        {
            get
            {
                if (_Insees == null) _Insees = new Insees(this.connexion);
                return _Insees;
            }
            set
            {
                _Insees = value;
            }
        }
        private Insees _Insees;

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

        public EdiSites EdiSites
        {
            get
            {
                if (_EdiSites == null) _EdiSites = new EdiSites(this.connexion);
                return _EdiSites;
            }
            set
            {
                _EdiSites = value;
            }
        }
        private EdiSites _EdiSites;

        public EdiQualifications EdiQualifications
        {
            get
            {
                if (_EdiQualifications == null) _EdiQualifications = new EdiQualifications(this.connexion);
                return _EdiQualifications;
            }
            set
            {
                _EdiQualifications = value;
            }
        }
        private EdiQualifications _EdiQualifications;

        public Tables Tables
        {
            get
            {
                if (_Tables == null) _Tables = new Tables(this.connexion);
                return _Tables;
            }
            set
            {
                _Tables = value;
            }
        }
        private Tables _Tables;

        public MdxOrganisations MdxOrganisations
        {
            get
            {
                if (_MdxOrganisations == null) _MdxOrganisations = new MdxOrganisations(this.connexion);
                return _MdxOrganisations;
            }
            set
            {
                _MdxOrganisations = value;
            }
        }
        private MdxOrganisations _MdxOrganisations;
    }
}
