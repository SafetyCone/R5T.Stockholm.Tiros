using System;
using System.IO;

using Microsoft.Extensions.Options;

using R5T.Stockholm.Default;
using R5T.Tiros;

using R5T.T0064;


namespace R5T.Stockholm.Tiros
{
    [ServiceImplementationMarker]
    public class TextStreamSerializer<T> : IStreamSerializer<T>, IServiceImplementation
    {
        private ITextSerializer<T> TextSerializer { get; }
        private IOptions<StreamSerializerOptions<T>> StreamSerializerOptions { get; }


        public TextStreamSerializer(ITextSerializer<T> textSerializer, IOptions<StreamSerializerOptions<T>> streamSerializerOptions)
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
                var streamWriter = this.StreamSerializerOptions.Value.AddByteOrderMark
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
