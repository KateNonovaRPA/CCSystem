using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels.LawsuitsData
{
    public class RobotData
    {
        public UserVM user { get; set; }
        public JusticeBgLawsuit justiceBgLawsuit { get; set; }   
        public SRSLawsuit SRSLawsuit { get; set; }  
        public VKSLawsuit VKSLawsuit{ get; set; }   
    }
}
