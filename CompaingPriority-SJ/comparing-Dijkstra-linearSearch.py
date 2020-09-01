import sys
# from heapq import heappush, heappop, heapify

class Vertex:
	def __init__(self, id):
		self.id = id
		self.adj = []
		self.visited = False
		self.inheap = False
		self.costSum = sys.maxsize
		self.parent = None
		self.name = ""

	def __ge__(self, other):
		return self.costSum >= other.costSum

	def __gt__(self, other):
		return self.costSum > other.costSum
		
	def __le__(self, other):
		return self.costSum <= other.costSum

	def __lt__(self, other):
		return self.costSum < other.costSum
		
	# sj for compatibility to those no weight graph
	def addNeighbour(self, vertex):
		self.addEdge(Edge(vertex, sys.maxint))

	
	def addEdge(self, edge):
		self.adj.append(edge)
  
	def toString(self):
		if self.parent == None :
			s = "{}[{},{}]: ".format(self.id, self.costSum, self.parent)
		else:
			s = "{}[{},{}]: ".format(self.id, self.costSum, self.parent.id)
		for edge in self.adj:
			s += " {}({})".format(edge.destId, edge.weight)
		return s		

class Edge:
	def __init__(self, dstId, wt):
		self.destId = dstId
		self.weight = wt

# linear search min
def minPop(vList):
	vMin = vList[0]
	vMinId = 0
	vId = 0
	for v in vList[1:]:
		vId += 1
		if v < vMin : 
			vMin = v
			vMinId = vId
	return vList.pop(vMinId)



class Graph:
	def __init__(self):
		self.vertices = []

	def printWAdj(self):
		for v in self.vertices:
			print(v.toString())

	def shortest(self, iStart, iEnd):
		vheap = []
		vStart = self.vertices[iStart]
		vStart.costSum = 0
		vheap.append(vStart) #sj no sort
		vStart.inheap = True

		while vheap:
			v = minPop(vheap)
			v.visited = True
			v.inheap = False
			if v.id == iEnd: 
				break #sjdb destiny is met
			for edge in v.adj:
				nb = self.vertices[edge.destId]
				if nb.visited :
					continue
				if v.costSum + edge.weight < nb.costSum:
					nb.costSum = edge.weight + v.costSum
					nb.parent = v
					# print("pushed: "+ nb.toString()) #sjdb pushed it if it is not exist in the heap, otherwise should update it. 
					if nb.inheap == False:

						vheap.append(nb)		
					nb.inheap = True

		vDest = self.vertices[iEnd]
		sumCost = vDest.costSum
		if sumCost >= sys.maxsize : 
			return sumCost   #sj error, the destiny might not connect to the src

		return sumCost
	



def main():
    line = sys.stdin.readline()
    nTestCases = int(line)
    
    #cId is case id
    for cId in range(nTestCases):
        line = sys.stdin.readline()
        parts = line.split()
        nV = int(parts[0]) #sj number of vertices, 0 based graph
        nE = int(parts[1]) #sj number of edges

        g = Graph()
        for id in range(nV):
            g.vertices.append(Vertex(id))

        line = sys.stdin.readline()
        parts = line.split()
        iSrc = int(parts[0])
        iDest = int(parts[1])

        for edge in range(nE):
            edgeLine = sys.stdin.readline()
            edgeParts = edgeLine.split()
            iStart = int(edgeParts[0])
            iEnd = int(edgeParts[1])
            iCost = int(edgeParts[2])
            g.vertices[iStart].addEdge(Edge(iEnd,iCost))
            g.vertices[iEnd].addEdge(Edge(iStart, iCost)) #sj!!!!! undirected graph

        shortestCost = g.shortest(iSrc, iDest)
        if shortestCost >= sys.maxsize:
            print("Case {}: {}".format(cId, "No path") )
        else:
            print("Case {}: {}".format(cId, shortestCost))
        

if __name__ == "__main__":
    main()