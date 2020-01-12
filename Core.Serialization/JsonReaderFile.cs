using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Core.Serialization
{
    public class JsonReaderFile<T> where T : class
    {
        private readonly IStringSerializer<T> _serializer;
        public JsonReaderFile(IStringSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        private static IEnumerable<IEnumerable<byte>> ReadFile(string file)
        {
            int countArray = 0;
            int countObject = 0;
            const byte comma = 44;
            const byte arrayStart = 91;
            const byte arrayEnd = 93;
            const byte objectStart = 123;
            const byte objectEnd = 125;
            const byte space = 32;
            const byte tab = 9;
            const byte CR = 10;
            const byte LF = 13;

            var buffer = new byte[1024];
            var chars = new List<byte>();
            using (var stream = File.OpenRead(file))
            {
                int countToRead = stream.Read(buffer, 0, buffer.Length);
                while (countToRead > 0)
                {
                    foreach (var c in buffer.Take(countToRead))
                    {
                        switch (c)
                        {
                            case space:
                            case tab:
                            case CR:
                            case LF:
                                break;
                            case objectStart:
                                chars.Add(c);
                                countObject++;
                                break;
                            case objectEnd:
                                chars.Add(c);
                                countObject--;
                                break;
                            case arrayStart:
                                if (++countArray > 1)
                                    chars.Add(c);
                                break;
                            case arrayEnd:
                                if (--countArray > 0)
                                    chars.Add(c);
                                break;
                            case comma:
                                if (countObject == 0 && countArray == 1)
                                {
                                    yield return chars.ToArray();
                                    chars = new List<byte>();
                                }
                                else
                                {
                                    chars.Add(c);
                                }
                                break;
                            default:
                                if (countObject != 0 || countArray != 0)
                                    chars.Add(c);
                                break;
                        }
                    }
                    countToRead = stream.Read(buffer, 0, buffer.Length);
                }
                if (chars.Any())
                    yield return chars.ToArray();
            }
        }

        private static IObservable<IEnumerable<byte>> ReadObservableFile(string file)
            => ReadFile(file).ToObservable();

        private async static IAsyncEnumerable<IEnumerable<byte>> ReadFileAsync(string file)
        {
            int countArray = 0;
            int countObject = 0;
            const byte comma = 44;
            const byte arrayStart = 91;
            const byte arrayEnd = 93;
            const byte objectStart = 123;
            const byte objectEnd = 125;
            const byte space = 32;
            const byte tab = 9;
            const byte CR = 10;
            const byte LF = 13;

            var buffer = new byte[1024];
            var chars = new List<byte>();
            using (var stream = File.OpenRead(file))
            {
                int countToRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                while (countToRead > 0)
                {
                    foreach (var c in buffer.Take(countToRead))
                    {
                        switch (c)
                        {
                            case space:
                            case tab:
                            case CR:
                            case LF:
                                break;
                            case objectStart:
                                chars.Add(c);
                                countObject++;
                                break;
                            case objectEnd:
                                chars.Add(c);
                                countObject--;
                                break;
                            case arrayStart:
                                if (++countArray > 1)
                                    chars.Add(c);
                                break;
                            case arrayEnd:
                                if (--countArray > 0)
                                    chars.Add(c);
                                break;
                            case comma:
                                if (countObject == 0 && countArray == 1)
                                {
                                    yield return chars.ToArray();
                                    chars = new List<byte>();
                                }
                                else
                                {
                                    chars.Add(c);
                                }
                                break;
                            default:
                                if (countObject != 0 || countArray != 0)
                                    chars.Add(c);
                                break;
                        }
                    }
                    countToRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (chars.Any())
                    yield return chars.ToArray();
            }
        }

        private T ToTResult(IEnumerable<byte> bytes)
            => _serializer.Deserialize(new string(bytes.Select(Convert.ToChar).ToArray()));

        public async IAsyncEnumerable<T> ReadValuesAsync(string file)
        {
            await foreach (var part in ReadFileAsync(file))
                yield return ToTResult(part);
        }

        public IEnumerable<T> ReadValues(string file)
            => ReadFile(file).Select(ToTResult);
    }
}
