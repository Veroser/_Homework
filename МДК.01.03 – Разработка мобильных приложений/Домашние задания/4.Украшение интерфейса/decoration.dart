import 'package:flutter/material.dart';

class Decor extends StatelessWidget {
  const Decor({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: Scaffold(
        body: Container(
          margin: EdgeInsets.all(20),
          padding: EdgeInsets.all(15),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [Colors.orange, Colors.red],
            ),
            borderRadius: BorderRadius.circular(15),
            boxShadow: [
              BoxShadow(
                color: Colors.grey,
                blurRadius: 10,
              ),
            ],
          ),
          child: Row(
            children: [
              Container(
                decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(8),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.3),
                    blurRadius: 15,
                    spreadRadius: 2,
                    offset: Offset(0, 5),
                    ),
                  ],
                ),
                padding: EdgeInsets.all(8),

                child: Text('#12345'),
              ),
               SizedBox(width: 5),
              Expanded(child: 
              Text('Доставка сегодня')),
              SizedBox(width: 15),
                   Container(
                decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(8),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.3),
                    blurRadius: 15,
                    spreadRadius: 2,
                    offset: Offset(0, 5),
                    ),
                  ],
                 ),
                 padding: EdgeInsets.all(8),
                 child: Column(
                  mainAxisSize: MainAxisSize.min,
                 children: [ Text(
                  '2,5 км',
                   style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold
                   ),
                   ),
                    Text(
                    'до вас', 
                  style: TextStyle(
                    fontSize: 12
                  )
                  )
                 ]
                ),
              ),
             SizedBox(width: 15),
              Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(8),
                  boxShadow: [
                    BoxShadow(
                    color: Colors.black.withOpacity(0.3),
                    blurRadius: 15,
                    spreadRadius: 2,
                    offset: Offset(0, 5),
                    )
                  ],
                ),
                padding: EdgeInsets.all(8),
                child: Icon(Icons.location_on),
              ),   
            ],
          ),
        )
      ),
    );
  }
}