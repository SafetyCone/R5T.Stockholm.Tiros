using System;
using System.IO;

using R5T.Magyar.IO;
using R5T.Tiros;


namespace R5T.Stockholm.Tiros
{
    public class TextStreamSerializer<T> : IStreamSerializer<T>
    {
        private ITextSerializer<T> TextSerializer { get; }


        public TextStreamSerializer(ITextSerializer<T> textSerializer)
        {
            this.TextSerializer = textSerializer;
        }

        public T Deserialize(Stream stream)
        {
            using (var textReader = StreamReaderHelper.NewLeaveOpen(stream))
            {
                var value = this.TextSerializer.Deserialize(textReader);
                return value;
            }
        }

        public void Serialize(Stream stream, T value)
        {
            using (var textWriter = StreamWriterHelper.NewLeaveOpen(stream))
            {
               this.TextSerializer.Serialize(textWriter, value);
            }
        }
    }
}
