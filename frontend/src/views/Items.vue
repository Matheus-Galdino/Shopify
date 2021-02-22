<template>
  <div id="items-tab">
    <header>
      <h1>
        <span class="yellow"> Shoppingify </span>
        allows you take your
        <br />
        shopping list wherever you go
      </h1>
    </header>

    <main>
      <section class="item-group" v-for="group in allItems" :key="group.name">
        <h2>{{ group.name }}</h2>

        <div class="items">
          <div class="item" v-for="item in group.items" :key="item.id">
            <p @click="$emit('selectItem', item)">
              {{ item.name }}
            </p>
            <span class="material-icons" @click="$emit('add-to-list', item)"> add </span>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>

<script>
export default {
  name: "Items",  
  props: {
    allItems: {
      type: Array,
      required: true,
    },  
  },
};
</script>

<style scoped>
.yellow {
  color: #f9a109;
}


#items-tab{
  padding-bottom: 4rem;
}

header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

header h1 {
  font-size: 26px;
}

main {
  margin-right: 9rem;
}

.item-group {
  margin-top: 5rem;
}

.item-group h2 {
  font-weight: 500;
  font-size: 2rem;
  margin-bottom: 2rem;
}

.item-group .items {
  display: flex;
  flex-wrap: wrap;
  column-gap: 2rem;
  row-gap: 4.5rem;
  align-items: center;
}

.item-group .item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-basis: 23%;

  padding: 1.5rem;

  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0px 2px 12px rgba(0, 0, 0, 0.05);

  cursor: pointer;
}
.item p {
  flex-grow: 1;
  font-weight: 500;
  font-size: 1.6rem;
}
.item .material-icons {
  color: #c1c1c4;
  cursor: pointer;
}
</style>
