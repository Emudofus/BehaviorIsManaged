using System.Text;

namespace NLog.LayoutRenderers.Wrappers
{
    [LayoutRenderer("blockcenter")]
    [AmbientProperty("Length")]
    public class BlockCenterLayoutRendereWrapper : WrapperLayoutRendererBase
    {
        public int Length
        {
            get;
            set;
        }

        public BlockCenterLayoutRendereWrapper()
        {
            
        }

        protected override string Transform(string text)
        {
            var builder = new StringBuilder(text);

            if (text.Length > Length)
            {
                var count = text.Length - Length + 3;
                builder.Remove(text.Length - count, count);
                builder.Append("...");
            }
            else if (Length > text.Length)
            {
                var fillersCount = Length - text.Length;

                for (int i = 0; i < fillersCount; i++)
                {
                    if (i >= fillersCount / 2)
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Insert(0, ' ');
                    }
                }
            }

            return builder.ToString();
        }
    }
}