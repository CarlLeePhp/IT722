import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Using Material',
      theme: ThemeData(
        primarySwatch: Colors.amber,
      ),
      home: HomeWidget(),
    );
  }
}

class HomeWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      child:  Scaffold(
        appBar: AppBar(
          title: Text('My Home'),
        ),
        body: TabBarView(
          children: <Widget>[HomeBody(), Contacts()],
        ) ,
        bottomNavigationBar: Container(
          child: TabBar(
            tabs: <Widget>[
              Tab(text: 'Home', icon: Icon(Icons.home),),
              Tab(text: 'Info', icon: Icon(Icons.info))
            ],
          ),
        ),
      ),
    );
  }
}

List<String> info = [
  'Dog Wanted',
  'Little Dog',
  'Can I throw it a bit?',

];

TextStyle standardTextStyle = TextStyle(
  fontSize: 14,
  color: Colors.deepOrange,
  fontFamily: 'Arial'
);
class Contacts extends StatelessWidget{
  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text(
        'Information Page',
        textDirection: TextDirection.ltr,
      ),
    );
  }
}
class HomeBody extends StatelessWidget{
  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    List<Widget> list = List<Widget>();
    list.add(Text(info[0], style: standardTextStyle,));

    Image myImage = Image.asset(
      'images/Dog.png',
      height: 200,
      width: 100,
      fit: BoxFit.fitWidth,
    );
    list.add(myImage);
    for (int i = 1;i<info.length;i++){
      list.add(Text(info[i], style: standardTextStyle,));
    }
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: list)
      ],
    );
  }
}

