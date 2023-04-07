using PortfolioMaster.Models;
using PortfolioMaster.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;

namespace PortfolioMaster.Services
{
    public class AssetService
    {
        private readonly ApplicationDbContext _context;

        public AssetService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Asset> GetAllAssetsForUser(string userId)
        {
            return _context.Assets.Where(a => a.UserId == userId).ToList();
        }

        public Asset GetAssetById(int id)
        {
            return _context.Assets.FirstOrDefault(a => a.Id == id);
        }

        public void CreateAsset<T>(T asset) where T : Asset
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
        }

        public void UpdateAsset<T>(T asset) where T : Asset
        {
            _context.Entry(asset).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteAsset(int id)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                _context.SaveChanges();
            }
        }

    }
}
