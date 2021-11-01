import React from 'react'
import BirdCard from './BirdCard'

export default function BirdList({birds}) {

  let birdCards = birds?.map(bird => (<BirdCard bird={bird}></BirdCard>))

  return (
    <div>
      {birdCards}
    </div>
  )
}
