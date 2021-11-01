import React from 'react'

export default function BirdCard({bird}) {
  return (
    <div>
      Type: {bird.type} <br/>
      Color: {bird.color} <br/>
      Name: {bird.name} <br/>
    </div>
  )
}
