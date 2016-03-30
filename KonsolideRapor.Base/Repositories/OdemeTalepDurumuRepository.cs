using KonsolideRapor.Base.Manage;
using KonsolideRapor.Base.Model.Entities;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolideRapor.Base.Repositories
{
    public class OdemeTalepDurumuRepository : GenericRepository<OdemeTalepDurumu>
    {
        #region Constructor
        //Hangi context kullanılacaksa burada(constructor da) belirtiyoruz. 
        public OdemeTalepDurumuRepository(KonsolideRaporContext contextParameter)
            : base(contextParameter.ApplicationContext.DBContext)
        {
            this.context = contextParameter;
        }

        #endregion

        #region Private Members

        private KonsolideRaporContext context;

        #endregion

        #region Public Members

        public KonsolideRaporContext Context
        {
            get { return context; }
        }

        #endregion
    }
}
