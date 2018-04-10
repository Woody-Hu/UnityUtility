using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityUtility;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

           
        }
    }

  
    [InterfaceAOP]
    public interface IA
    {
        [ExceptionAop]
        string Test();
    }

    [Compent(RegistByClass = false)]
    public class A : IA
    {
        public string Test()
        {
            throw new NotImplementedException();
        }
    }





}
