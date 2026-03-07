using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; private set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategoryInfo { get; set; }

        // This constructor for EF Core to can create instance from Category class
        private Category() { }

        // Constructor
        public Category(int categoryId,Category parentCategoryInfo)
        {
            this.CategoryID = categoryId;
            this.ParentCategoryInfo = parentCategoryInfo;
        }
    }
}
