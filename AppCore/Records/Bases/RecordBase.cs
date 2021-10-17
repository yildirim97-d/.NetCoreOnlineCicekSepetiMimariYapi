using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Records.Bases
{
    /*
     -Abstract class olmalıdır.(new gerek yok)
     -Tüm entityler için ortak olan özellik.
     */

    public abstract class RecordBase
    {
        public int Id { get; set; }
        public string Guid { get; set; }
    }
}
