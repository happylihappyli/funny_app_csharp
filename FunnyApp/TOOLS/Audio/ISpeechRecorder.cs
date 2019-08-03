using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.TOOLS.Audio {

    public interface ISpeechRecorder {
        void SetFileName(string fileName);
        void StartRec();
        void StopRec();
    }
}
