using System;
using System.Collections.Generic;
using System.Web;
using Db4objects.Db4o.Collections;

namespace MyShows.Core.Models
{
    public class Profile
    {
        public Profile(int id)
        {
            UserId = id;
        }
        public Profile()
            : this(-1)
        {
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        private ActivatableList<Series> _series;
        public ActivatableList<Series> Series
        {
            get { return _series ?? (_series = new ActivatableList<Series>()); }
            set { _series = value; }
        }
    }
}