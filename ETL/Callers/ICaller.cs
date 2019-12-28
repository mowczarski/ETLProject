using DataContext.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Callers
{
    interface ICaller
    {
        void AddMovie(Movie movie);
    }
}
