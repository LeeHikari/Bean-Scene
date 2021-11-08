using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Member.Controllers

{[Area("Member"), Authorize(Roles = "Member")]

public abstract class 
    MemberAreaBaseController : Controller
{
        protected readonly ApplicationDbContext _context;

        public MemberAreaBaseController(ApplicationDbContext context)
        {
            _context = context;
        }
   }
}
