using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specflow.Sample.StepArgumentTransformations
{
    [Binding]
    public class NullableEmptyTransformations
    {
        [StepArgumentTransformation("<NULL>")]
        public static string? NullTransformation()
        {
            return null;
        }

        [StepArgumentTransformation("<EMPTY>")]
        public static string EmptyTransformation()
        {
            return string.Empty;
        }
    }
}
