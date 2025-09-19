using Xim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xim.Domain.Repos
{
    public class MenuWithSubMenu
    {
        public MenuEntity Menu { get; set; }
        public List<MenuSubEntity> SubMenu { get; set; }
    }
}
