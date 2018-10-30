using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
	public interface ICommentService
	{
		Task<IEnumerable<Comment>> GetCommentsByBillToShipTo(int billTo, int shipTo, int centerNumber);
        Task<IEnumerable<Comment>> GetCommentsByRoutePlanId(int routePlanId, int centerNumber);
        Task<IEnumerable<Comment>> GetCommentsByRoutePlanIds(int[] routePlanIds, int centerNumber);
        Task<Comment> GetByCommentIdAsync(int commentId);
        Task UpdateCommentAsync(UpdateComment comment);
        Task AddCommentAsync(AddComment comment);
    }
}