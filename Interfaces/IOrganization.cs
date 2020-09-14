using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Interfaces
{
	/// <summary>
	/// Interface alias of Organization
	/// Necessary to separate UI and back-end
	/// Also implements access to positions of organization
	/// </summary>
	public interface IOrganization
	{
		/// <summary>
		/// Organization name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Date of establishment of the organization
		/// </summary>
		DateTime EstablishedOn { get; set; }

		/// <summary>
		/// List of positions in the company. 
		/// It is necessary to access to positions via indexer.
		/// IPositions provides indexer access to positions
		/// </summary>
		IPositions PositionsData { get; }

		/// <summary>
		/// Returns positions available for specified department
		/// </summary>
		/// <param name="dept">Department</param>
		/// <param name="keepDirector">Keep director in positins list or not</param>
		/// <returns></returns>
		List<IPositionTuple> Available(IDepartmentDTO dept, bool keepDirector);
	}
}
