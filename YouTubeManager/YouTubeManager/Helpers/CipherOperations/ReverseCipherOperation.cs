using YouTubeManager.Extensions;

namespace YouTubeManager.Helpers.CipherOperations
{
    internal class ReverseCipherOperation : ICipherOperation
    {
        public string Decipher(string input)
        {
            return input.Reverse();
        }
    }
}