using HashidsNet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Shared.encryptservice
{
    public interface IHashIdService
    {
        string EncodeId(int id);
        string EncodeId(long id);
        string EncodeIds(params int[] ids);
        int DecodeToInt(string hashedId);
        long DecodeToLong(string hashedId);
        int[] DecodeToInts(string hashedId);
        bool TryDecodeToInt(string hashedId, out int id);
        bool TryDecodeToLong(string hashedId, out long id);
    }
    public class HashIdService : IHashIdService
    {
        private readonly IHashids _hashIds;
        private readonly IHashids _longHashIds;

        public HashIdService(IConfiguration configuration)
        {
            var salt = configuration["HashIds:Salt"] ?? "DefaultSaltForERP";
            var minLength = configuration.GetValue<int>("HashIds:MinLength", 8);
            var alphabet = configuration["HashIds:Alphabet"] ?? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            _hashIds = new Hashids(salt, minLength, alphabet);
            _longHashIds = new Hashids($"{salt}_long", minLength, alphabet);
        }

        public string EncodeId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than 0", nameof(id));

            return _hashIds.Encode(id);
        }

        public string EncodeId(long id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than 0", nameof(id));

            return _longHashIds.EncodeLong(id);
        }

        public string EncodeIds(params int[] ids)
        {
            if (ids == null || ids.Length == 0)
                throw new ArgumentException("IDs array cannot be null or empty", nameof(ids));

            if (ids.Any(id => id <= 0))
                throw new ArgumentException("All IDs must be greater than 0", nameof(ids));

            return _hashIds.Encode(ids);
        }

        public int DecodeToInt(string hashedId)
        {
            if (string.IsNullOrWhiteSpace(hashedId))
                throw new ArgumentException("Hashed ID cannot be null or empty", nameof(hashedId));

            var decoded = _hashIds.Decode(hashedId);

            if (decoded.Length == 0)
                throw new ArgumentException("Invalid hashed ID format", nameof(hashedId));

            return decoded[0];
        }

        public long DecodeToLong(string hashedId)
        {
            if (string.IsNullOrWhiteSpace(hashedId))
                throw new ArgumentException("Hashed ID cannot be null or empty", nameof(hashedId));

            var decoded = _longHashIds.DecodeLong(hashedId);

            if (decoded.Length == 0)
                throw new ArgumentException("Invalid hashed ID format", nameof(hashedId));

            return decoded[0];
        }

        public int[] DecodeToInts(string hashedId)
        {
            if (string.IsNullOrWhiteSpace(hashedId))
                throw new ArgumentException("Hashed ID cannot be null or empty", nameof(hashedId));

            var decoded = _hashIds.Decode(hashedId);

            if (decoded.Length == 0)
                throw new ArgumentException("Invalid hashed ID format", nameof(hashedId));

            return decoded;
        }

        public bool TryDecodeToInt(string hashedId, out int id)
        {
            id = 0;

            if (string.IsNullOrWhiteSpace(hashedId))
                return false;

            try
            {
                var decoded = _hashIds.Decode(hashedId);
                if (decoded.Length > 0)
                {
                    id = decoded[0];
                    return true;
                }
            }
            catch
            {
                // Ignore exceptions in Try methods
            }

            return false;
        }

        public bool TryDecodeToLong(string hashedId, out long id)
        {
            id = 0;

            if (string.IsNullOrWhiteSpace(hashedId))
                return false;

            try
            {
                var decoded = _longHashIds.DecodeLong(hashedId);
                if (decoded.Length > 0)
                {
                    id = decoded[0];
                    return true;
                }
            }
            catch
            {
                // Ignore exceptions in Try methods
            }

            return false;
        }
    }

    public static class HashIdExtensions
    {
        public static string ToHashId(this int id, IHashIdService hashIdService)
        {
            return hashIdService.EncodeId(id);
        }

        public static string ToHashId(this long id, IHashIdService hashIdService)
        {
            return hashIdService.EncodeId(id);
        }

        public static int FromHashId(this string hashedId, IHashIdService hashIdService)
        {
            return hashIdService.DecodeToInt(hashedId);
        }

        public static long FromHashIdToLong(this string hashedId, IHashIdService hashIdService)
        {
            return hashIdService.DecodeToLong(hashedId);
        }
    }
}
