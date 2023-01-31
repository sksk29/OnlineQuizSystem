using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
   public class assignment_answers
    {
		public int assign_answers_id { get; set; }
		public int assignment_id { get; set; }
		public int student_id { get; set; }
	
		public string student_ans { get; set; }
		public int gain_marks { get; set; }
		public string guid { get; set; }

		public string created_at { get; set; }

		public int? created_by { get; set; }

		public string modified_at { get; set; }
		public int? modified_by { get; set; }
		public int? is_deleted { get; set; }

		public string resultMsg { get; set; }

		public bool IsSuccess { get; set; }

		public string ReturnURL { get; set; }
		public string ErrorMsg { get; set; }

		public int TotalRecordCount { get; set; }
		public int pageId { get; set; }
		public int ItemsPerPage { get; set; }
		public string created_by_username { get; set; }
	}
}
