They are labs of the Paper IT722

#### Toplogical Sort

+ Use the Vertex struct from the Adjacency list exercise.
+ Add an int field to the Vertex struct for a tally of incomming edges.
+ Add lists of int for both a "starting list" and a "sorted list"
+ Construct an adjacency list, keeping a count of incoming edges for each vertex.
+ Construct a list (start-list) of nodes with no incoming edges.
+ These tasks do not depend on other tasks.
+ Define an empty list (sorted-list) to hold the sorted list.

While start-list is not empty

- Remove a vertex
- Add the vertex to sorted-list
- For each neighbour, remove the edge from the graph (decrement tally)
- If the neighbour has no other incoming edges add it to the start list.

Example Input:
3
go
ready
set
1 2
2 0

Output:
ready
set
go