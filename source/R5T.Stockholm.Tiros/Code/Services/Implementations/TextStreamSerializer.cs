using System;
using System.IO;

using R5T.Magyar.IO;
using R5T.Stockholm.Default;
using R5T.Tiros;


namespace R5T.Stockholm.Tiros
{
    public class TextStreamSerializer<T> : IStreamSerializer<T>
    {
        private ITextSerializer<T> TextSerializer { get; }
        private StreamSerializerOptions<T> StreamSerializerOptions { get; }


        public TextStreamSerializer(ITextSerializer<T> textSerializer, StreamSerializerOptions<T> streamSerializerOptions)
        {
            this.TextSerializer = textSerializer;
            this.StreamSerializerOptions = streamSerializerOptions;
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
            StreamWriter GetStreamWriter()
            {
                var streamWriter = this.StreamSerializerOptions.AddByteOrderMark
                    ? StreamWriterHelper.NewLeaveOpenAddBOM(stream)
                    : StreamWriterHelper.NewLeaveOpen(stream)
                    ;

                return streamWriter;
            }

            using (var textWriter = GetStreamWriter())
            {
               this.TextSerializer.Serialize(textWriter, value);
            }
        }
    }
}
