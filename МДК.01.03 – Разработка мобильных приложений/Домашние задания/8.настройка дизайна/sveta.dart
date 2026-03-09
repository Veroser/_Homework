import 'package:flutter/material.dart';
import 'package:getwidget/getwidget.dart';

class Sveta extends StatelessWidget {
  const Sveta({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: Scaffold(
        body: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              const GFAvatar(
                size: 80,
                child: Icon(Icons.person, size: 60, color: Colors.white,),
                backgroundColor: Colors.blueGrey,
              ),
              const SizedBox(height: 16),
              const Text(
                'Светлана',
                style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
              ),
              const Text(
                'Преподователь рисования',
                style: TextStyle(fontSize: 16, color: Colors.grey),
              ),
              const SizedBox(height: 24),
              GFRating(
                value: 4,
                itemCount: 5,
                size: GFSize.SMALL,
                color: Colors.amber,
                borderColor: Colors.amber,
                allowHalfRating: true,
                onChanged: (value) {},
              ),
              const SizedBox(height: 24),
              GFListTile(
                titleText: 'Email',
                subTitleText: 'sveta245645643@gmail.com',
                icon: const Icon(Icons.email),
              ),
              const SizedBox(height: 24),
              GFListTile(
                titleText: 'Phone',
                subTitleText: '88005553535',
                icon: const Icon(Icons.phone),
              ),
            ],
          ), 
        ),
      ),
    );
  }
}