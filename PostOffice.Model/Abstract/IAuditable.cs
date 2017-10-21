using System;

namespace PostOffice.Model.Abstract
{
    public interface IAuditable
    {
        DateTime? CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        string MetaKeyWord { get; set; }
        string MetaDescription { get; set; }
        bool Status { get; set; }
    }
}