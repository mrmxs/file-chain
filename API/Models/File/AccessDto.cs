using System;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Models.File
{
    public class AccessDto
    {
        public int? Id { get; set; }
        public int Owner { get; set; }
        public int[] Editors { get; set; }
        public int[] Viewers { get; set; }
        
        public static AccessDto Stub()
        {
            return new AccessDto
            {
                Id = 4,
                Owner = 125,
                Editors = new int[]{},
                Viewers = new int[]{126, 3},
            };
        }
    }
}