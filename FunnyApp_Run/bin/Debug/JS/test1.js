function Point (x, y) {
    this.x = x;
    this.y = y;
}

function Tools(){
    
    this.distance = function (a, b) {
        const dx = a.x - b.x;
        const dy = a.y - b.y;

        return Math.sqrt(dx*dx+dy*dy);
    }
}
var p1 = new Point(5, 5);
var p2 = new Point(10, 10);


var tools=new Tools();

sys.Button_Init("b1","Test",10,50,200,30,"show_distance","");

sys.Add_Text("txt1","输入信息",10,10,200,30);

function show_distance(){
    sys.Show_Text("txt1","distance=:"+tools.distance(p1, p2));
}

sys.Show_Form(300,300);

